using System;
using System.Diagnostics;
using System.Linq;
using Model.Abstracts;
using Util.Collections;
using Util.Extensions;

namespace Model.Generation;

internal class MetaCollector (MetaModel mm)
{

    private readonly Type matterType = typeof(Matter);
    private readonly Type familyType = typeof(Family<Matter>);
    private readonly Type refType = typeof(Ref<Matter>);
    private readonly Type polyRefType = typeof(PolyRef<Matter>);

    internal void CollectMetaData()
    {
        foreach (Type intf in ModelMetaBrief.AllModelMatters)
        {
            HandleInterface(intf);
        }
    }

    private void HandleInterface(Type intf)
    {
        Debug.Assert(intf.IsInterface);
        Debug.Assert(intf.IsIn(ModelMetaBrief.AllModelMatters));

        bool isConcrete = intf.IsIn(ModelMetaBrief.ConcreteModelMatters);
        var  m          = new MetaMatter(intf, isConcrete);
        HandleBaseInterfaces(intf,
                             out m.DeclaredBaseIntfs,
                             out m.IndirectBaseIntfs,
                             out m.AllBaseIntfs);
        mm.Add(m);
    }

    private void HandleBaseInterfaces(Type              intf,
                                      out ImmList<Type> declaredBaseIntfs,
                                      out ImmSet<Type>  indirectBaseIntfs,
                                      out ImmSet<Type>  allBaseIntfs)
    {
        ImmList<Type> allBaseInterfaces =
            intf.GetInterfaces()
                .Where(i => i.IsAssignableTo(matterType))
                .ToImmList();
        ImmSet<Type> indirectBaseInterfaces =
            allBaseInterfaces
               .SelectMany(i => i.GetInterfaces())
               .ToImmSet();
        ImmList<Type> declaredBaseInterfaces =
            allBaseInterfaces
               .Where(i => i.IsNotIn(indirectBaseInterfaces))
               .ToImmList();
        foreach (var i in declaredBaseInterfaces)
            Debug.Assert(i.IsIn(mm.Intfs.Keys));
        declaredBaseIntfs = declaredBaseInterfaces;
        indirectBaseIntfs = indirectBaseInterfaces;
        allBaseIntfs = allBaseInterfaces.ToImmSet();
    }

}