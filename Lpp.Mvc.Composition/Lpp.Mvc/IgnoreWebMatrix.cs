// This file is here to work around a Razor bug that causes the code generator to accidentally
// add "using WebMatrix.Data" and "using WebMatrix.WebData" to generated files
// See here for more info: http://stackoverflow.com/questions/4146545/razor-helper-in-mvc-3-rc/4148467#4148467
// TODO: Remove this when the issue is resolved

namespace WebMatrix.Data { internal class Ignore { } }
namespace WebMatrix.WebData { internal class Ignore { } }