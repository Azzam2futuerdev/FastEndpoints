﻿using Grpc.Core;
using Grpc.Net.Client;

namespace FastEndpoints;

internal interface IServerStreamCommandExecutor<TResult> : ICommandExecutor where TResult : class
{
    IAsyncStreamReader<TResult> ExecuteServerStream(IServerStreamCommand<TResult> command, CallOptions opts);
}

internal sealed class ServerStreamCommandExecutor<TCommand, TResult> : BaseCommandExecutor<TCommand, TResult>, IServerStreamCommandExecutor<TResult>
    where TCommand : class, IServerStreamCommand<TResult>
    where TResult : class
{
    public ServerStreamCommandExecutor(GrpcChannel channel)
        : base(channel: channel,
               methodType: MethodType.ServerStreaming)
    { }

    public IAsyncStreamReader<TResult> ExecuteServerStream(IServerStreamCommand<TResult> cmd, CallOptions opts)
        => _invoker.AsyncServerStreamingCall(_method, null, opts, (TCommand)cmd).ResponseStream;
}