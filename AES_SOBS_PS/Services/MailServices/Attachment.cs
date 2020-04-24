using System;
using System.IO;
using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.MailServices
{
    public class Attachment
    {
        private readonly string path;

        public Attachment()
        {

        }
        public Attachment(string path)
        {
            this.path = path;
        }
        public Attachment(string fileName, object content)
        {
            FileName = fileName;
            Content = content;
        }



        public string FileName { get; private set; }
        public object Content { get; }
        public string Path { get { return path; } }

        public enum AttachmentType { Json, Text }

        public AttachmentType Type { get; set; }


        public async Task<MemoryStream> ContentToStreamAsync()
        {
            string text;
            switch (Type)
            {
                case AttachmentType.Json:
                    text = Newtonsoft.Json.JsonConvert.SerializeObject(Content);
                    break;
                case AttachmentType.Text:
                    text = Content.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            await writer.WriteAsync(text);
            await writer.FlushAsync();
            stream.Position = 0;
            return stream;
        }

        public async Task<MemoryStream> GetContentFromPath()
        {
            try
            {
                FileInfo archivo = new FileInfo(path);
                if (!archivo.Exists)
                    throw new FileNotFoundException();

                FileName = archivo.Name;

                MemoryStream ms;
                using (ms = new MemoryStream())
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    await file.ReadAsync(bytes, 0, (int)file.Length);
                    await ms.WriteAsync(bytes, 0, (int)file.Length);
                }

                return ms;

            }
            catch (FileNotFoundException fnex)
            {
                throw fnex;
            }
        }

        public async Task<FileStream> GetContentAsync()
        {
            try
            {
                FileInfo archivo = new FileInfo(path);
                if (!archivo.Exists)
                    throw new FileNotFoundException();

                FileName = archivo.Name;

                FileStream file;
                using (file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    await file.ReadAsync(bytes, 0, (int)file.Length);      
                }

                return file;

            }
            catch (FileNotFoundException fnex)
            {
                throw fnex;
            }
        }


    }
}
