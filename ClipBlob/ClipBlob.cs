using System;
using System.Threading;
using Azure.Storage.Blobs;

namespace ClipBlob
{
    public class ClipBlob
    {
        private string _clipboard;
        private string _remoteBlob;
        private BlobClient _blobClient;

        protected string GetClipboard()
        {
            return Clipboard.GetText();
        }

        protected void SetClipboard(string item)
        {
            Clipboard.SetText(item);
        }

        private string GetRemoteBlob()
        {
            if (_blobClient == null)
            {
                throw new InvalidOperationException();
            }

            return _blobClient.DownloadContent().Value.Content.ToString();
        }

        private void SetRemoteBlob(string item)
        {
            if (_blobClient == null)
            {
                throw new InvalidOperationException();
            }

            _blobClient.Upload(new BinaryData(item), true);
        }

        private bool IsClipBoardChanged()
        {
            var item = GetClipboard();
            if (_clipboard != item)
            {
                _clipboard = item;
                return true;
            }

            return false;
        }

        private bool IsRemoteBlobChanged()
        {
            var item = GetRemoteBlob();
            if (_remoteBlob != item)
            {
                _remoteBlob = item;
                return true;
            }

            return false;
        }

        public void SetBlobUri(string uri)
        {
            _blobClient = new BlobClient(new Uri(uri));
            if (!_blobClient.Exists())
            {
                throw new ArgumentException("Invalid blob uri");
            }
        }

        public void Listen()
        {
            while (true)
            {
                if (IsRemoteBlobChanged())
                {
                    Console.WriteLine("Remote Blob change detected");
                    SetClipboard(_remoteBlob);
                    _clipboard = _remoteBlob;
                }

                if (IsClipBoardChanged())
                {
                    Console.WriteLine("Clipboard change detected");
                    SetRemoteBlob(_clipboard);
                    _remoteBlob = _clipboard;
                }

                Thread.Sleep(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}