namespace Model.Abstracts;


public class ModelMaster : ModelAccessor
{
    private uint IdCounter;

    public ModelMaster(uint version) : base(version)
    {

    }

    public uint NewId()
    {
        return ++IdCounter;
    }

}
