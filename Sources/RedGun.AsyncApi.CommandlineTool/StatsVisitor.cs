using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;

namespace RedGun.AsyncApi.CommandlineTool {
   internal class StatsVisitor : AsyncApiVisitorBase
    {
        // TODO: Delete here what we no longer need
        
        public int ParameterCount { get; set; } = 0;

        public override void Visit(AsyncApiParameter parameter)
        {
            ParameterCount++;
        }

        public int SchemaCount { get; set; } = 0;

        public override void Visit(AsyncApiSchema schema)
        {
            SchemaCount++;
        }

        public int HeaderCount { get; set; } = 0;

        public override void Visit(IDictionary<string, AsyncApiHeader> headers)
        {
            HeaderCount++;
        }

        public int ChannelItemCount { get; set; } = 0;

        public override void Visit(AsyncApiChannelItem channelItem)
        {
            ChannelItemCount++;
        }
        
        public int PathItemCount { get; set; } = 0;
        
        public override void Visit(AsyncApiPathItem pathItem)
        {
            PathItemCount++;
        }

        public int RequestBodyCount { get; set; } = 0;

        public override void Visit(AsyncApiRequestBody requestBody)
        {
            RequestBodyCount++;
        }

        public int ResponseCount { get; set; } = 0;

        public override void Visit(AsyncApiResponses response)
        {
            ResponseCount++;
        }

        public int OperationCount { get; set; } = 0;

        public override void Visit(AsyncApiOperation operation)
        {
            OperationCount++;
        }

        public int LinkCount { get; set; } = 0;

        public override void Visit(AsyncApiLink operation)
        {
            LinkCount++;
        }

        public int CallbackCount { get; set; } = 0;

        public override void Visit(AsyncApiCallback callback)
        {
            CallbackCount++;
        }

        public string GetStatisticsReport()
        {
            return $"Path Items: {PathItemCount}" + Environment.NewLine
                 + $"Operations: {OperationCount}" + Environment.NewLine
                 + $"Parameters: {ParameterCount}" + Environment.NewLine
                 + $"Request Bodies: {RequestBodyCount}" + Environment.NewLine
                 + $"Responses: {ResponseCount}" + Environment.NewLine
                 + $"Links: {LinkCount}" + Environment.NewLine
                 + $"Callbacks: {CallbackCount}" + Environment.NewLine
                 + $"Schemas: {SchemaCount}" + Environment.NewLine;
        }
    }
}