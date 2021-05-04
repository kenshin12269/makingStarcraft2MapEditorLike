using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public interface IListenToPostRenderCallback
    {
        void OnPostRender();
    }
    public class PostRenderCallbackReceiver : MonoBehaviour
    {
        List<IListenToPostRenderCallback> list = new List<IListenToPostRenderCallback>();
        public void Listen(IListenToPostRenderCallback listener)
        {
            list.Add(listener);
        }
        public void Remove(IListenToPostRenderCallback listener)
        {
            list.Remove(listener);
        }
        void OnPostRender()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].OnPostRender();
            }
        }
    }
}
