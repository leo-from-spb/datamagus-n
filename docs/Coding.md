Code Conventions
================


Indentations and Braces
-----------------------

We use 4 spaces for indentation.

In C# files we don't use tab characters, but in some other files we can use them.

The rule for braces and parentheses: the closing brace/paren must be at a column no less than the column of the opening one.
Example:
```C#
public class MyClass
{
    void MyMethod()
    {
        CallSomeMethod(parameter1, parameter2, parameter3);
        CallMediumMethod(parameter1,
                         parameter2,
                         parameter3);
        for (int i = 0; i < 10; i++) { DoSomethingShort(i); }
        for (int i = 0; i < 10; i++) { DoSomethingMedium(i); 
                                       DoSomethingElse(i); }
        for (int i = 0; i < 10; i++) 
        { 
            DoSomethingLongs(i); 
            DoSomethingElse(i); 
        }
    }
}    
```


Naming Convention
-----------------

We use the regular C# naming convention with some special home rules.

* For namespaces, classes, methods, properties, constants: PascalCase.
* For local variables and parameters: camelCase.
* For type parameters: one or two upper letters or one upper letter followed by a digit.
* For enumeration items: abbItemName, where abb is an abbreviation of the enumeration name.
    * Exception: for measurement units, where an item is naturally written right after a number with a dot (e.g. `5.mm`), the prefix is omitted and the item name is just the unit abbreviation itself.
* For test methods: combined names like TestMethodName_CaseName_SpecialOption — a test name is a combination of 1 or more PascalCase parts delimited by an underscore.

We do NOT use screaming caps.
We do NOT use the 'I' prefix for interfaces (no exceptions please!).
There is no syntactic prefix or suffix that distinguishes interfaces from classes either — they are simply given different names.
Prefixes such as `Ab`, `Con`, `Dia` used in the model are *semantic* (they describe the domain role of a type) and may appear on both classes and interfaces.

We use english letters in the production code (for names and comments), 
but we can use also German or Cyrillic letters in tests (not often).


Properties and Fields
---------------------

When a backing field is needed inside a property, use the `field` contextual keyword instead of declaring a separate private field.


Namespaces
----------

Use file-scoped namespace declarations (`namespace Foo;`); do not wrap the file in `namespace Foo { ... }`.


Other Rules
-----------

* `var` is allowed; use it where it improves readability.
* There is no hard line-length limit. As a soft target, try to keep lines within ~100 characters.

         
