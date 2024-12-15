using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FileSignatureValidator
{
    public interface IFileAnalyzer
    {
        void Analyze(string directoryPath);
    }

    public interface ISignatureProvider
    {
        Dictionary<string, string> LoadSignatures(string jsonPath);
    }

    public class SignatureProvider : ISignatureProvider
    {
        public Dictionary<string, string> LoadSignatures(string jsonPath)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO AO CARREGAR BANCO DE DADOS DE ASSINATURA: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }
    }

    public class FileAnalyzer : IFileAnalyzer
    {
        private readonly ISignatureProvider _signatureProvider;

        public FileAnalyzer(ISignatureProvider signatureProvider)
        {
            _signatureProvider = signatureProvider;
        }

        public void Analyze(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"DIRETÓRIO NÃO EXISTE: {directoryPath}");
                return;
            }

            string jsonPath = "fileSignatures.json";
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"BANCO DE DADOS DE ASSINATURA NÃO ENCONTRADO: {jsonPath}");
                return;
            }

            var signatures = _signatureProvider.LoadSignatures(jsonPath);
            if (signatures == null || signatures.Count == 0)
            {
                Console.WriteLine("NENHUMA ASSINATURA ENCONTRADA NO DB.");
                return;
            }

            Console.WriteLine("ANALISANDO ARQUIVOS DO DIRETORIO: " + directoryPath);
            var files = Directory.GetFiles(directoryPath);

            foreach (var file in files)
            {
                AnalyzeFile(file, signatures);
            }
        }
        private void AnalyzeFile(string filePath, Dictionary<string, string> signatures)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[8];
                    fs.Read(buffer, 0, buffer.Length);
                    string fileSignature = BitConverter.ToString(buffer).Replace("-", string.Empty);
                    var matchedSignature = signatures.FirstOrDefault(sig => fileSignature.StartsWith(sig.Key));
                    string extension = Path.GetExtension(filePath).TrimStart('.').ToUpper();
                    string fileName = Path.GetFileName(filePath);

                    if (!string.IsNullOrEmpty(matchedSignature.Key))
                    {
                        string expectedType = matchedSignature.Value;

                        if (!expectedType.Equals(extension, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"[ALERTA] {fileName}: ESPERADO .{expectedType}, MAS FOI ENCONTRADO .{extension}.");
                        }
                        else
                        {
                            Console.WriteLine($"[OK] {fileName}: TIPO DE ARQUIVO VALIDO");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[DESCONHECIDO] {fileName}: Tipo do Arquivo desconhecido. Assinatura: {fileSignature}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao analisar arquivo {filePath}: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: FileSignatureValidator <directory_path>");
                return;
            }

            string directoryPath = args[0];

            ISignatureProvider signatureProvider = new SignatureProvider();
            IFileAnalyzer fileAnalyzer = new FileAnalyzer(signatureProvider);

            fileAnalyzer.Analyze(directoryPath);
        }
    }
}
