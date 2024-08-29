     |***************************************************************************|
     |                                                                           |
     |   IN ORDER FOR THIS PROJECT TO WORK,                                      |
	 |                                                                           |
     |     1. ADD REFERENCES TO:                                                 |
     |          Lpp.Mvc.dll                                                      |
     |          Lpp.Composition.dll                                              |
     |          Lpp.Mvc.Composition.dll                                          |
     |          System.ComponentModel.Composition.CodePlex.dll                   |
     |          System.ComponentModel.Composition.Registration.CodePlex.dll      |
     |          System.Reflection.Context.CodePlex.dll						     |
	 |                                                                           |
     |     2. SET "Output Path" TO THE                                           |
	 |        "bin" FOLDER OF YOUR ROOT                                          |
	 |        WEB PROJECT                                                        |
     |                                                                           |
     |***************************************************************************|

This is a starter template for a MEF-composable part for an MVC project.
It relies on the following LPP libraries: 
	Lpp.Mvc
	Lpp.Composition
	Lpp.Mvc.Composition

These libraries are still in development/stabilization phase,
therefore, they are not included as a ready-to-use package.
For now, you have to manually add references to them.

As for the output path, I still have to figure out how to
analyze the solution, find the web project and get its bin folder path.
The "Output Path" can be set manually from Project->Properties->Build