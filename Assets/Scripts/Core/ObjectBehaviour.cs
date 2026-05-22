using UnityEngine;

public abstract class ObjectBehaviour<INSTANCE, STATE, DATA> : MonoBehaviour
        where INSTANCE : ObjectInstance<STATE, DATA>
        where STATE : ObjectState<DATA>
        where DATA : ObjectData
{
    protected INSTANCE instance;

    public INSTANCE Instance => instance;
    public DATA Data => instance.state.data;
    public STATE State => instance.state;

    public void SetInstance(INSTANCE instance)
    {
        this.instance = instance;
        InstanceCreated();
    }

    protected abstract void InstanceCreated();
}
