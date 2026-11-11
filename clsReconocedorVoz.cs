using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Envoltura del reconocedor de voz moderno de Windows 10/11 (Windows.Media.SpeechRecognition).
    // Es el que se instala con "Reconocimiento de voz básico" en Configuración → Idioma.
    // Solo reconoce las frases que se le pasan (gramática cerrada) y funciona sin internet.
    // Los eventos llegan en otro hilo: la pantalla debe pasarlos a su propio hilo.
    public sealed class clsReconocedorVoz : IDisposable
    {
        private SpeechRecognizer reconocedor;
        private bool preparado;

        // Windows no permite preparar/iniciar/detener a la vez: los pedidos se atienden de uno en uno.
        private readonly SemaphoreSlim turno = new SemaphoreSlim(1, 1);

        public string Idioma { get; private set; }

        public bool Escuchando { get; private set; }

        public event Action<string, decimal> FraseReconocida;

        public event Action<string> EscuchaTerminada;

        public static string BuscarIdiomaEspanol()
        {
            try
            {
                Language idioma = SpeechRecognizer.SupportedGrammarLanguages
                    .FirstOrDefault(l => l.LanguageTag.StartsWith("es", StringComparison.OrdinalIgnoreCase));
                return idioma == null ? null : idioma.LanguageTag;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Carga la gramática cerrada (las frases autorizadas). Devuelve null si todo salió bien,
        /// o un mensaje explicando el problema.
        /// </summary>
        public async Task<string> PrepararAsync(IEnumerable<string> frases)
        {
            await turno.WaitAsync();
            try
            {
                await DetenerSinTurnoAsync();

                if (reconocedor == null)
                {
                    string tag = BuscarIdiomaEspanol();
                    if (tag == null)
                        return "Windows no tiene reconocimiento de voz en español. Instala Español (México) con \"Reconocimiento de voz\".";

                    reconocedor = new SpeechRecognizer(new Language(tag));
                    reconocedor.Timeouts.InitialSilenceTimeout = TimeSpan.FromSeconds(8);
                    reconocedor.Timeouts.EndSilenceTimeout = TimeSpan.FromMilliseconds(800);
                    reconocedor.ContinuousRecognitionSession.AutoStopSilenceTimeout = TimeSpan.FromSeconds(15);
                    reconocedor.ContinuousRecognitionSession.ResultGenerated += Sesion_ResultGenerated;
                    reconocedor.ContinuousRecognitionSession.Completed += Sesion_Completed;
                    Idioma = tag;
                }

                List<string> lista = frases.Where(f => !string.IsNullOrWhiteSpace(f)).Distinct().ToList();
                if (lista.Count == 0) return "No hay frases para reconocer.";

                reconocedor.Constraints.Clear();
                reconocedor.Constraints.Add(new SpeechRecognitionListConstraint(lista, "comandos"));

                SpeechRecognitionCompilationResult compilacion = await reconocedor.CompileConstraintsAsync();
                preparado = compilacion.Status == SpeechRecognitionResultStatus.Success;

                return preparado ? null : "No se pudo preparar el reconocimiento (" + compilacion.Status + ").";
            }
            catch (Exception ex)
            {
                preparado = false;
                return Traducir(ex);
            }
            finally
            {
                turno.Release();
            }
        }

        /// <summary>Empieza a escuchar. Devuelve null si pudo, o el mensaje del problema.</summary>
        public async Task<string> IniciarAsync()
        {
            await turno.WaitAsync();
            try
            {
                if (reconocedor == null || !preparado) return "El reconocimiento de voz no está listo.";
                if (Escuchando) return null;

                Escuchando = true;
                await reconocedor.ContinuousRecognitionSession.StartAsync();
                return null;
            }
            catch (Exception ex)
            {
                Escuchando = false;
                return Traducir(ex);
            }
            finally
            {
                turno.Release();
            }
        }

        /// <summary>Deja de escuchar; lo último que se dijo todavía se procesa.</summary>
        public async Task DetenerAsync()
        {
            await turno.WaitAsync();
            try
            {
                await DetenerSinTurnoAsync();
            }
            finally
            {
                turno.Release();
            }
        }

        // Detiene la escucha; quien lo llama ya tiene el turno.
        private async Task DetenerSinTurnoAsync()
        {
            if (reconocedor == null || !Escuchando) return;

            try
            {
                await reconocedor.ContinuousRecognitionSession.StopAsync();
            }
            catch (Exception)
            {
                // Si ya se había detenido sola, no pasa nada.
            }
            finally
            {
                Escuchando = false;
            }
        }

        private void Sesion_ResultGenerated(SpeechContinuousRecognitionSession sender,
                                            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            SpeechRecognitionResult r = args.Result;
            bool rechazado = r.Status != SpeechRecognitionResultStatus.Success ||
                             r.Confidence == SpeechRecognitionConfidence.Rejected;

            string texto = rechazado ? string.Empty : r.Text;
            decimal confianza = Escala(r.Confidence, r.RawConfidence);

            Action<string, decimal> manejador = FraseReconocida;
            if (manejador != null) manejador(texto ?? string.Empty, confianza);
        }

        private void Sesion_Completed(SpeechContinuousRecognitionSession sender,
                                      SpeechContinuousRecognitionCompletedEventArgs args)
        {
            Escuchando = false;

            string mensaje = null;
            switch (args.Status)
            {
                case SpeechRecognitionResultStatus.Success:
                case SpeechRecognitionResultStatus.UserCanceled:
                    break;
                case SpeechRecognitionResultStatus.TimeoutExceeded:
                    mensaje = "No se escuchó nada. Mantén pulsado y habla.";
                    break;
                case SpeechRecognitionResultStatus.MicrophoneUnavailable:
                    mensaje = "No hay micrófono disponible.";
                    break;
                case SpeechRecognitionResultStatus.AudioQualityFailure:
                    mensaje = "La calidad del audio es mala. Acércate al micrófono.";
                    break;
                default:
                    mensaje = "El reconocimiento se detuvo (" + args.Status + ").";
                    break;
            }

            Action<string> manejador = EscuchaTerminada;
            if (manejador != null) manejador(mensaje);
        }

        /// <summary>
        /// Convierte la confianza de Windows a la escala 0-100 del sistema.
        /// Con listas cerradas, RawConfidence suele quedar entre 0.4 y 0.8 aunque la frase sea correcta,
        /// así que se usa el nivel que da Windows (Alta / Media / Baja) como base:
        ///   Alta  → 96 % (pasa todos los niveles, incluso N4 que pide 95 %)
        ///   Media → 88 % (pasa N1/N2 con umbral 85 %; no alcanza N3/N4, que piden 90 % o más)
        ///   Baja  → 65 % (se rechaza y se pide repetir)
        /// Si RawConfidence es mayor que ese valor, se usa RawConfidence.
        /// </summary>
        public static decimal Escala(SpeechRecognitionConfidence nivel, double crudo)
        {
            decimal baseNivel;
            switch (nivel)
            {
                case SpeechRecognitionConfidence.High: baseNivel = 96m; break;
                case SpeechRecognitionConfidence.Medium: baseNivel = 88m; break;
                case SpeechRecognitionConfidence.Low: baseNivel = 65m; break;
                default: baseNivel = 0m; break;
            }

            decimal valorCrudo = (decimal)Math.Round(Math.Max(0.0, Math.Min(1.0, crudo)) * 100.0, 1);
            return Math.Max(baseNivel, valorCrudo);
        }

        // Mensajes claros para los errores típicos de Windows.
        private static string Traducir(Exception ex)
        {
            switch ((uint)ex.HResult)
            {
                case 0x80045509:
                    return "Windows no permite el reconocimiento de voz: activa Configuración → Privacidad y seguridad → Voz → \"Reconocimiento de voz en línea\".";
                case 0x80070005:
                    return "Windows bloqueó el micrófono: activa Configuración → Privacidad y seguridad → Micrófono → \"Permitir que las aplicaciones de escritorio accedan al micrófono\".";
                case 0x8004503A:
                    return "El idioma de voz no está instalado completamente. Reinstala \"Reconocimiento de voz\" para Español.";
                default:
                    return "Error del reconocimiento de voz: " + ex.Message;
            }
        }

        public void Dispose()
        {
            if (reconocedor == null) return;

            try
            {
                reconocedor.ContinuousRecognitionSession.ResultGenerated -= Sesion_ResultGenerated;
                reconocedor.ContinuousRecognitionSession.Completed -= Sesion_Completed;
                reconocedor.Dispose();
            }
            catch (Exception) { }

            reconocedor = null;
        }
    }
}
