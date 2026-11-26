namespace Brimborium.Angular;

public static class FrontendLocation {
  public static string GetFrontendWebRootPath() {
    var location = GetProjectDirectory();
    string result;
#if DEBUG
    result = System.IO.Path.Combine(location, "dist", "development", "ui");
#else
    result = System.IO.Path.Combine(location, "dist", "production", "ui");
#endif
    return result;
  }

  public static string GetFrontendProject() {
    var location = GetProjectDirectory();
    return System.IO.Path.Combine(location, "Brimborium.AngularUI.csproj");
  }

  private static string GetProjectDirectory([System.Runtime.CompilerServices.CallerFilePath] string filePath = "") {
    return System.IO.Path.GetDirectoryName(filePath) ?? throw new Exception();
  }
}
