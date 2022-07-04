namespace ClassLibrary2;

public class LoadRequest<T> : LoadRequest,ILoadRequest<T> where T : class
{
    private string loadFileName;

    public LoadRequest(string fileName)
    {
        this.loadFileName = fileName;
    }

    public Type assetType {
        get
        {
            return typeof(T);
        }
    }
    public string fileName {
        get
        {
            return loadFileName;
        }
    }
    public string Error { get; protected set; }

    public void SetRequest(object obj)
    {
        Result = obj as T;
        Completed = true;
    }

    public void SetError(string error)
    {
        Error = error;
        Completed = true;
    }

    public bool Completed { get; protected set; }
    public T Result { get; protected set; }
}