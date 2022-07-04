using System.Collections;

namespace ClassLibrary2;

public class LoadManager
{
    private static LoadManager _instance = null;

    public static LoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoadManager();
                _instance.Init();
            }

            return _instance;
        }
    }

    public virtual ILoadRequest<string> LoadText(string fileName)
    {
        return LoadAsset<string>(fileName);
    }

    public virtual ILoadRequest<byte[]> LoadRequest(string fileName)
    {
        return LoadAsset<byte[]>(fileName);
    }

    public virtual ILoadRequest<T> LoadAsset<T>(string fileName) where T : class
    {
        var request = new LoadRequest<T>(fileName);
        if (string.IsNullOrEmpty(_rootPath))
        {
            Console.WriteLine("[LoadManager] You Should Do LoadManager.SetRoorPath() First");
            return request;
        }

        foreach (var loader in _loaders)
        {
            if(loader.Invoke(request)){
                return request;
            }
        }
        
        request.SetError($"[LoadManager] Has no loader to load the typeof: <{typeof(T)}>");

        return request;
    }

    List<Loader> _loaders = new List<Loader>();

    public void AddLoader<T>(Action<LoadRequest> loader)
    {
        if(loader == null)
            return;
        
        _loaders.Add(new Loader(){type = typeof(T),loader = loader});
    }

    public void Init()
    {
        AddLoader<string>(request=>CoroutineManager.Instance.Start(LoadAsyncImpl(request,File.ReadAllText)));
        
        AddLoader<byte[]>(request=>CoroutineManager.Instance.Start(LoadAsyncImpl(request,File.ReadAllBytes)));
    }

    private string _rootPath;

    public string RootPath {
        get
        {
            return _rootPath;
        }
    }

    /// <summary>
    /// 初始化根目录
    /// 所有文件完整路径为: rootPath + "\\" + fileName
    /// </summary>
    /// <param name="rootPath"></param>
    public void SetRootPath(string rootPath)
    {
        _rootPath = rootPath;
    }

    private IEnumerator LoadAsyncImpl(LoadRequest request, Func<string, object> syncLoadFunc)
    {
        var req = Task.Run(() =>
        {
            try
            {
                return syncLoadFunc(_rootPath + "/" + request.fileName);
            }
            catch (Exception e)
            {
                return e;
            }
        });

        yield return req;
        
        if(req.Result is Exception)
            request.SetError((req.Result as Exception).Message);
        else
        {
            request.SetRequest(req.Result);
        }
    }
}