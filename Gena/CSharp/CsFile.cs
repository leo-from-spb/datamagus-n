using System;
using System.Collections.Generic;
using Gena.Code;
using Gena.Fun;
using Util.Extensions;
using static Gena.CSharp.CsVisibility;

namespace Gena.CSharp;

/// <summary>
/// C# file.
/// </summary>
public class CsFile
{
    public readonly CsConstruction Con;

    /// <summary>
    /// Namespace.
    /// </summary>
    public string Namespace;

    /// <summary>
    /// File name (without path and extension).
    /// </summary>
    public string Name;


    /// <summary>
    /// The text that is placed at the top if the file.
    /// </summary>
    public string Header = CsConsts.DefaultHeader;


    public readonly SortedSet<string> Imports = new SortedSet<string>();

    public readonly List<CsClass> Classes = new();

    internal CsFile(CsConstruction con, string ns, string name)
    {
        Con       = con;
        Namespace = ns;
        Name      = name;
    }

    public CsClass NewClass(string name,
                            string? baseNames = null,
                            CsVisibility visibility = visPublic,
                            bool isStatic = false,
                            bool isAbstract = false,
                            bool isSealed = false)
    {
        var newClass = new CsClass(this, name, baseNames, isStatic, isAbstract, isSealed);
        newClass.Visibility = visibility;
        Classes.Add(newClass);
        return newClass;
    }
}


/// <summary>
/// A C# named entity (class, method, variable, etc.).
/// </summary>
public abstract class CsEntity
{
    /// <summary>
    /// Name of the entity.
    /// </summary>
    public string Name;

    /// <summary>
    /// Visibility of the entity.
    /// </summary>
    public CsVisibility Visibility;

    /// <summary>
    /// C# Documentation.
    /// </summary>
    public string? Documentation = null;


    protected CsEntity(string name, CsVisibility visibility = visAuto)
    {
        Name       = name;
        Visibility = visibility;
    }

    public override string ToString()
    {
        var className = this.GetType().Name.RemovePrefix("Cs");
        return $"{className} {Name}";
    }
}


/// <summary>
/// C# class.
/// </summary>
public class CsClass : CsEntity
{
    public readonly CsFile File;

    public string? BaseNames;

    public bool IsStatic;
    public bool IsAbstract;
    public bool IsSealed;

    /// <summary>
    /// Type parameters.
    /// </summary>
    public readonly List<string> TypoParams = new();

    /// <summary>
    /// Names of interfaces this class implements.
    /// </summary>
    public readonly List<string> ImplementInterfaces = new();

    /// <summary>
    /// Group of entities.
    /// </summary>
    public readonly List<CsGroup> Groups = new();

    /// <summary>
    /// The first group.
    /// This group is used by default.
    /// </summary>
    public readonly CsGroup FirstGroup = new();

    public string? WrappingRegion = null;


    internal CsClass(CsFile file, string name, string? baseNames, bool isStatic, bool isAbstract, bool isSealed)
        : base(name)
    {
        File = file;
        BaseNames = baseNames;
        IsStatic = isStatic;
        IsAbstract = isAbstract;
        IsSealed = isSealed;

        Groups.Add(FirstGroup);

        if (Visibility.IsAuto)
            Visibility = visPublic;
    }


    public CsGroup NewGroup(string? region)
    {
        var group = new CsGroup();
        group.WrappingRegion = region;
        Groups.Add(group);
        return group;
    }

    public CsField NewField(CsGroup?     group,
                            string       fieldName,
                            string       fieldType,
                            string?      defaultValue = null,
                            string?      expression   = null,
                            CsVisibility visibility   = visAuto)
    {
        var g = group ?? FirstGroup;
        var f = new CsField(fieldName, fieldType, defaultValue, expression, visibility);
        g.Fields.Add(f);
        return f;
    }

    public CsConstructor NewConstructor(CsGroup? group)
    {
        var g   = group ?? FirstGroup;
        var ctr = new CsConstructor(this);
        g.Constructors.Add(ctr);
        return ctr;
    }

    public CsMethod NewMethod(CsGroup? group,
                              string methodName,
                              string?  returnType = null,
                              bool     isStatic   = false,
                              bool     isAbstract = false)
    {
        var g = group ?? FirstGroup;
        var method = new CsMethod(this, methodName, returnType, isStatic, isAbstract);
        g.Methods.Add(method);
        return method;
    }
}


/// <summary>
/// Group of entities inside a class.
/// </summary>
public class CsGroup
{
    /// <summary>
    /// Region.
    /// </summary>
    public string? WrappingRegion;

    /// <summary>
    /// Comment before the group content.
    /// </summary>
    public string? Comment;

    public readonly List<CsField>       Fields       = new();
    public readonly List<CsConstructor> Constructors = new();
    public readonly List<CsMethod>      Methods      = new();

    public bool Some    => Fields.Some || Constructors.Some || Methods.Some;
    public bool IsEmpty => !Some;
}


/// <summary>
/// C# Field or Property.
/// </summary>
public class CsField : CsEntity
{
    public string  Type;
    public string? DefaultValue;
    public string? TheGetExpression;
    public string? TheSetExpression;

    public bool IsStatic;
    public bool IsReadOnly;
    public bool IsOverride;

    public CsField(string       name,
                   string       type,
                   string?      defaultValue = null,
                   string?      expression   = null,
                   CsVisibility visibility   = visAuto)
        : base(name, visibility)
    {
        Type = type;
        DefaultValue = defaultValue;
        TheGetExpression = expression;

        if (Visibility.IsAuto)
            Visibility = name.IsCapitalized ? visPublic : IsOverride ? visProtected : visPrivate;
    }

    /// <summary>
    /// It is a plain field (not a property) and has no accessors.
    /// </summary>
    public bool IsPlainField => TheGetExpression is null && TheSetExpression is null;

    /// <summary>
    /// It is a pure expression (property with only a getter).
    /// </summary>
    public bool IsPureExpression => DefaultValue is null && TheGetExpression.Some && TheSetExpression is null;

    /// <summary>
    /// It is a pure property with empty get and possible empty set.
    /// </summary>
    public bool IsPureProperty => TheGetExpression == "" && (TheSetExpression is null || TheSetExpression == "");

    public override string ToString()
    {
        var s = $"Field {Type} {Name}";
        if (DefaultValue != null) s += $" = {DefaultValue}";
        return s;
    }
}


/// <summary>
/// C# Routine (Constructor or Method).
/// </summary>
public abstract class CsRoutine : CsEntity
{
    public readonly CsClass Class;

    public readonly List<CsArgument> Arguments = new();

    public readonly CodeBuilder ContentBuilder;

    protected CsRoutine(CsClass clazz, string name, CsVisibility visibility)
        : base(name, visibility)
    {
        Class           = clazz;
        ContentBuilder  = new CodeBuilder();

        ContentBuilder.Indentation = "    ";
    }

    public CsArgument NewArgument(string name, string type, string? defaultValue = null)
    {
        var parameter = new CsArgument(Class, name, type, defaultValue);
        Arguments.Add(parameter);
        return parameter;
    }

    public string Content => ContentBuilder.Result;
}


/// <summary>
/// C# Constructor.
/// </summary>
public class CsConstructor : CsRoutine
{
    /// <summary>
    /// Expressions that should be passed to "base" or "this" constructor.
    /// </summary>
    /// <seealso cref="PassToThis"/>
    public readonly List<string> PassArguments = new();

    /// <summary>
    /// How to cascade constructor: false -> base, true -> this.
    /// </summary>
    public bool PassToThis = false;

    public CsConstructor(CsClass clazz)
        : base(clazz, "", visAuto)
    {
        if (Visibility.IsAuto)
            Visibility = clazz.IsAbstract ? visProtected : clazz.Visibility;
    }
}


/// <summary>
/// C# Method (function).
/// </summary>
public class CsMethod : CsRoutine
{
    public bool IsStatic;
    public bool IsAbstract;
    public bool IsSealed;
    public bool IsExpression;

    public string? ReturnType;

    public CsMethod(CsClass clazz, string name, string? returnType, bool isStatic, bool isAbstract)
        : base(clazz, name, visPublic)
    {
        Name            = name;
        IsStatic        = isStatic;
        IsAbstract      = isAbstract;
        this.ReturnType = returnType;

        if (Visibility.IsAuto)
            Visibility = clazz.Visibility;
    }
}


/// <summary>
/// C# Argument.
/// </summary>
public class CsArgument
{
    public readonly CsClass Class;

    public string Name;
    public string Type;

    public string? DefaultValue;

    public CsArgument(CsClass clazz, string name, string type, string? defaultValue)
    {
        Class = clazz;
        Name  = name;
        Type  = type;
        DefaultValue = defaultValue;
    }

    public string Spec => Type + ' ' + Name + DefaultValue.Wrap(prefix: " = ");

}
