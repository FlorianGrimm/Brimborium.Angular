namespace Brimborium.Angular;

public static class FrontendLocation {
  public static string GetFrontendWebRootPath() {
    var location = GetProjectDirectory();
    return System.IO.Path.Combine(location, "dist", "development", "ui");
  }

  public static string GetFrontendProject() {
    var location = GetProjectDirectory();
    return System.IO.Path.Combine(location, "Brimborium.AngularUI.csproj");
  }

  private static string GetProjectDirectory([System.Runtime.CompilerServices.CallerFilePath] string filePath = "") {
    return System.IO.Path.GetDirectoryName(filePath) ?? throw new Exception();
  }
}
