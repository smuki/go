﻿using Newtonsoft.Json;
using Rabbit.Go.Codec;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Go.Core.Codec
{
    public class JsonCodec : ICodec
    {
        #region Implementation of ICodec

        /// <inheritdoc/>
        public IEncoder Encoder { get; } = new JsonEncoder();

        /// <inheritdoc/>
        public IDecoder Decoder { get; } = new JsonDecoder();

        #endregion Implementation of ICodec

        private class JsonEncoder : IEncoder
        {
            #region Implementation of IEncoder

            /// <inheritdoc/>
            public Task EncodeAsync(object instance, Type type, GoRequest request)
            {
                request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(instance)));
                return Task.CompletedTask;
            }

            #endregion Implementation of IEncoder
        }

        private class JsonDecoder : IDecoder
        {
            #region Implementation of IDecoder

            /// <inheritdoc/>
            public async Task<object> DecodeAsync(GoResponse response, Type type)
            {
                var reader = new StreamReader(response.Body);
                var content = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(content, type);
            }

            #endregion Implementation of IDecoder
        }
    }
}