using System;

namespace ClipBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            ClipBlob clipBlob = new ClipBlob();

            clipBlob.SetBlobUri(args[0]);
            clipBlob.Listen();
        }
    }
}
