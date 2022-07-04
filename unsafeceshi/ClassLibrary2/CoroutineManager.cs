using System.Collections;

namespace ClassLibrary2;

public class CoroutineManager
{
    private static CoroutineManager _instance = null;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CoroutineManager();
            return _instance;
        }
    }
    
    //链表存储所有协程对象
    private LinkedList<Coroutine> coroutineList = new LinkedList<Coroutine>();

    private LinkedList<Coroutine> coroutineToStop = new LinkedList<Coroutine>();
    
    //开启一个协程
    public Coroutine Start(IEnumerator ie)
    {
        var c = new Coroutine(ie);
        coroutineList.AddLast(c);
        return c;
    }
    
    //关闭一个协程
    public void Stop(IEnumerator ie)
    {
    }

    public void Stop(Coroutine coroutine)
    {
        coroutineToStop.AddLast(coroutine);
    }
    
    //主线程驱动所有协程对象
    public bool UpdateCoroutine()
    {
        var node = coroutineList.First;
        while (node != null)
        {
            var cor = node.Value;

            bool ret = false;
            if (cor != null)
            {
                bool toStop = coroutineToStop.Contains(cor);
                if (toStop)
                {
                    
                }

                if (!toStop)
                {
                    //一旦协程返回false,即意味着该协程要退出
                    ret = cor.MoveNext();
                }
            }

            if (!ret)
            {
                coroutineList.Remove(node);
                Console.WriteLine($"移除当前协程:{node.Value}");
                if(coroutineList.Count == 0)
                    return false;
            }

            node = node.Next;
        }
        return true;
    }
}