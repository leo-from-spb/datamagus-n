Project Layout
==============
       
Here is described the layout of the project and its modules, and other directories.
                              

Modules
-------

By a module we mean a "project" in terms of MS dot-Net solution.
       
We have three kinds of modules:
    * 'C' — production code (these modules contain code that will become a part of the final product),
    * 'G' — generators and tools (these modules are used during the development),
    * 'T' — tests (these modules contain tests and test infrastructure as well).
            
Modules:

| Module                                 | Kind  | Description                                       |
|----------------------------------------|-------|---------------------------------------------------|
| [Core](Core)                           | C     | Core interfaces and simple classes                |
| [Core_Imp](Core_Imp)                   | C     | Core implementation                               |
| [Core_Test](Core_Test)                 | T     | Tests for core modules                            |
| [Gena](Gena)                           | **G** | Gena the Crocodile — code genaration framework    |
| [Gena_Test](Gena_Test)                 | T     | Tests for Gena the Crocodile                      |
| [Model_Essence](Model_Essence)         | C     | Model definition                                  |
| [Model_Gen](Model_Gen)                 | **G** | Model generator (a command-line app)              |  
| [Model_Imp](Model_Imp)                 | C     | Model implementation (including generated code)   |
| [Model_Test](Model_Test)               | T     | Tests for model modules                           |
| [DataMagus_App](DataMagus_App)         | C     | Application bootstrap                             |
| [Gui_Application](Gui_Application)     | C     | Most GUI code (UI is based on Avalonia framework) |
| [Util](Util)                           | C     | Utility classes and functions                     |
| [Util_Test](Util_Test)                 | T     | Tests for utility module                          |
| [Testing_Appliance](Testing_Appliance) | T     | Test infrastructure                               |

      

Code Generation
---------------

Some model code is generated.
The generator is located in the 'Model_Gen' module.
The generated code is located in the module Model_Imp, in the [\_generated\_](Model_Imp/\_generated\_) directory.

To generate the code, run the 'Model_Gen' module that is a simple command-line application.
Or use the "Generate_Model" run configuration in Rider.

In order to run the generator, the following modules must be compilable:
* Gena
* Model_Essence
* Model_Gen
* Util                                                                   
             

Intermediate Files
------------------

Project building and compilation produces tons of intermediate files. In order to avoid cluttering the project directory,
most of them are located in the [yonder](yonder) directory.

