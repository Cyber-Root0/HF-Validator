File Signature Validator
========================

Descrição
---------

O **File Signature Validator** é uma aplicação de linha de comando (CLI) desenvolvida em C# que realiza a análise da integridade de arquivos em um diretório, validando se os arquivos possuem a extensão correta com base na assinatura binária (_magic number_) presente nos primeiros bytes do arquivo.

Ele verifica se a extensão do arquivo é consistente com o seu conteúdo real. Caso o tipo do arquivo não seja identificado ou a extensão seja incompatível, o programa exibe alertas e fornece informações para análise posterior.

* * *

Funcionalidades
---------------

*   **Validação de Assinaturas de Arquivos:** Verifica os primeiros bytes dos arquivos e os compara com uma base de dados JSON de assinaturas binárias conhecidas.
    
*   **Consistência da Extensão:** Analisa se a extensão do arquivo corresponde ao tipo real detectado.
    
*   **Identificação de Arquivos Desconhecidos:** Exibe a assinatura hexadecimal de arquivos não reconhecidos para análise manual ou futura inclusão na base de dados.
    
*   **Análise em Lote:** Processa todos os arquivos de um diretório especificado.
    

* * *

Estrutura de Arquivos
---------------------

1.  **Código Principal:** Implementação do programa em `C#`.
    
2.  **Base de Dados JSON:** Arquivo `fileSignatures.json` contendo os _magic numbers_ e os tipos correspondentes. Um exemplo:
    
    {
    
    "25504446": "PDF",
    
    "89504E47": "PNG",
    
    "FFD8FFE0": "JPG",
    
    "00000018": "MP4",
    
    "49443303": "MP3"
    
    }
    

* * *

Como Usar
---------

### Pré-requisitos

*   **.NET SDK** instalado em sua máquina.
    
*   Base de dados de assinaturas (`fileSignatures.json`) localizada no mesmo diretório do programa.
    

### Instalação

1.  Clone este repositório.
    
2.  Certifique-se de que o arquivo `fileSignatures.json` está presente no mesmo diretório do executável.
    
3.  Compile o programa utilizando o CLI do .NET:
    
    dotnet build
    

### Execução

1.  Navegue até o diretório do executável.
    
2.  Execute o programa no terminal:
    
    FileSignatureValidator <caminho\_do\_diretorio>
    
    Exemplo:
    
    FileSignatureValidator C:\\Users\\Bruno\\Documents\\TestFiles
    
3.  O programa analisará todos os arquivos no diretório especificado.
    

* * *

Exemplos de Saída
-----------------

### Caso de Sucesso

\[OK\] example.pdf: File type is valid.

### Aviso de Inconsistência

\[WARNING\] video.mp4: Expected .MP4, but found .AVI.

### Arquivo Não Reconhecido

\[UNKNOWN\] unknown.bin: Unrecognized file type. Signature: 3C68746D6C3E.

* * *

Boas Práticas Adotadas
----------------------

O código segue princípios de design, incluindo:

*   **SOLID:** Separação de responsabilidades em interfaces e classes específicas.
    
*   **Design Patterns:** Uso do padrão de injeção de dependência para facilitar a manutenção e escalabilidade.
    
*   **Legibilidade:** Divisão lógica em camadas para simplificar futuras melhorias.
    

* * *

Melhorias Futuras
-----------------

*   Expansão da base de dados de assinaturas binárias.
    
*   Suporte a análise recursiva em subdiretórios.
    
*   Inclusão de logs detalhados e opção de exportar relatórios.
    
*   Atualização dinâmica da base de dados JSON via API.
    

* * *

Contribuição
------------

Sinta-se à vontade para contribuir com melhorias no código ou na base de dados de assinaturas. Para contribuir:

1.  Realize um _fork_ deste repositório.
    
2.  Faça as alterações desejadas.
    
3.  Envie um _pull request_ com uma descrição detalhada.
    

* * *

Licença
-------

Este projeto é licenciado sob a licença MIT. Consulte o arquivo `LICENSE` para mais informações.