namespace ClassLibrary2;

public class Loader
{
    public Type type;

    public Action<LoadRequest> loader;

    public bool Invoke(LoadRequest request)
    {
        if (!type.IsAssignableFrom(request.assetType))
            return false;

        loader(request);
        return true;
    }
}