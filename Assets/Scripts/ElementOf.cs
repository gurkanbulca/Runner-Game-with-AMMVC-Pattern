using UnityEngine;

public abstract class ElementOf<T> : MonoBehaviour where T : MonoBehaviour
{
    protected T Master
    {
        get
        {
            if (_master == null)
            {
                _master = FindObjectOfType<T>();
            }

            if (_master == null)
            {
                var obj = new GameObject();
                obj.name = typeof(T).Name;
                _master = obj.AddComponent<T>();
            }
            

            return _master;
        }
    }

    private T _master;
}