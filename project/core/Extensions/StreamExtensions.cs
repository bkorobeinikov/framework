using System;
using System.Collections.Generic;
using System.IO;

namespace Bobasoft
{
    public static class StreamExtensions
    {
        //======================================================
        #region _Public methods_

        public static byte[] ToBytes(this Stream stream)
        {
            //var originalPosition = stream.Position;
            //stream.Position = 0;

            try
            {
                var bytes = new List<byte>();
                var buffer = new byte[2048];

                int readed;
                while ((readed = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var tempBuffer = new byte[readed];
                    Array.Copy(buffer, tempBuffer, tempBuffer.Length);
                    bytes.AddRange(tempBuffer);
                }

                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                ex.ToString();
                // TODO: log some error
            }
            finally
            {
                //stream.Position = originalPosition;
            }

            return null;
        }

        #endregion
    }
}