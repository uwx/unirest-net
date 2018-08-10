﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HSNXT.Unirest.Net.Entities.Exceptions;
using Newtonsoft.Json;

namespace HSNXT.Unirest.Net.Entities
{
    /// <inheritdoc cref="System.IDisposable" />
    /// <summary>
    /// Represents an unsuccessful HTTP response. The response body can be read in a format different to the intended
    /// one.
    ///
    /// <p>A <see cref="PartialHttpResponse{T}"/> object only needs to be disposed when it's contained in an
    /// <see cref="UnirestResponseException{T}"/>.</p>
    /// </summary>
    /// <typeparam name="T">The original target response type, which a failed response needn't conform to</typeparam>
    public class PartialHttpResponse<T> : BaseHttpResponse, IDisposable
    {
        private HttpContent Content { get; }

        internal PartialHttpResponse(BaseHttpResponse existing, HttpContent content)
            : base(existing.Headers, existing.CodeType, false)
        {
            Content = content;
        }

        /// <summary>
        /// Parses this response's body as the original JSON type, deserializing it with
        /// <see cref="JsonConvert.DeserializeObject{T}(string)"/>.
        /// </summary>
        /// <returns>The deserialized value as <typeparamref name="T"/></returns>
        public async Task<T> AsJsonAsync()
        {
            return JsonConvert.DeserializeObject<T>(await Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Parses this response's body as a defined JSON type, deserializing it with
        /// <see cref="JsonConvert.DeserializeObject{T}(string)"/>.
        /// </summary>
        /// <returns>The deserialized value as <typeparamref name="TTarget"/></returns>
        public async Task<TTarget> AsJsonAsync<TTarget>()
        {
            return JsonConvert.DeserializeObject<TTarget>(await Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Resolves to this response's body as a string.
        /// </summary>
        /// <returns>A Task resolving to this response's body as a string.</returns>
        public Task<string> AsStringAsync() => Content.ReadAsStringAsync();
        
        /// <summary>
        /// Resolves to this response's body's raw data as an array of bytes.
        /// </summary>
        /// <returns>A Task resolving to this response's body's raw data as an array of bytes.</returns>
        public Task<byte[]> AsByteArrayAsync() => Content.ReadAsByteArrayAsync();
        
        /// <summary>
        /// Resolves to this response's body's raw data as a <see cref="Stream"/>.
        /// </summary>
        /// <returns>A Task resolving to this response's body's raw data as a <see cref="Stream"/>.</returns>
        public Task<Stream> AsStreamAsync() => Content.ReadAsStreamAsync();

        public void Dispose() => Content?.Dispose();
    }
}