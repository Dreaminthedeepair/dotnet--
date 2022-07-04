using System.Collections;

namespace ClassLibrary2;

public class Coroutine : AsyncOp
{
    private IEnumerator _routine;

    public bool Completed { get; private set; }

    public Coroutine(IEnumerator routine)
    {
        _routine = routine;
        Completed = (routine == null);
    }

    public bool MoveNext()
    {
        if (_routine == null)
            return false;

        object current = _routine.Current;
        if (current is IWait)
        {
            IWait wait = (IWait)current;
            
            //检测等待条件,条件满足,跳到迭代器的下一元素(IEnumerator方法里的下一个yield)
            if (!wait.Tick())
            {
                return true;
            }
        }

        if (current != null)
        {
            if (!AsyncCompleteChecker.IsCompleted(current))
                return true;
        }

        Completed = !_routine.MoveNext();

        return !Completed;

        //看迭代器当前的流程控制(即yield return 后边的对象)
        //是否是我们当前IWait对象,如果是,看是否满足moveNext条件
        // IWait wait = _routine.Current as IWait;
        // bool moveNext = true;
        // if (wait != null)
        //     moveNext = wait.Tick();
        //
        // if (!moveNext)
        // {
        //     //当时间没有到时,返回true
        //     //告诉管理器我们后边还有对象需要下一次迭代
        //     Console.WriteLine("[Coroutine] not movenext");
        //     return true;
        // }
        // else
        // {
        //     //此时所有等待时间或者帧都已经迭代完毕,看IEnumerator对象后续是否还有 yield return对象
        //     //将此结果通知管理器,管理器会在下一次迭代时决定是否继续迭代该Coroutine对象
        //     
        //     Console.WriteLine("[Coroutine] movenext");
        //     return _routine.MoveNext();
        // }
        //
        // return _routine.MoveNext();
    }

    public void Stop()
    {
        _routine = null;
        Completed = true;
    }
}