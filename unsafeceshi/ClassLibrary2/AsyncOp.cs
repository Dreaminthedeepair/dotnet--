namespace ClassLibrary2;

public interface AsyncOp
{
    bool Completed { get; }
}

public interface AsyncOpEx : AsyncOp
{
    float Progress { get; }
}

public interface AsyncRequest<T> : AsyncOp
{
    T Result { get; }
}

public interface AsyncRequestEx<T> : AsyncRequest<T>, AsyncOpEx
{
    
}

/// <summary>
/// 异步加载的异步操作接口
/// </summary>
public interface ILoadRequest : AsyncOp
{
    string fileName { get; }
    string Error { get; }
}

public interface ILoadRequest<T> : AsyncRequest<T>, ILoadRequest where T : class
{
}

public interface LoadRequest
{
    Type assetType { get; }
    string fileName { get; }

    void SetRequest(object obj);
    void SetError(string error);
}
