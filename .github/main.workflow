workflow "Build and Test in PR" {
  on = "push"
  resolves = [".NET Core CLI"]
}

action ".NET Core CLI" {
  uses = "./"
  args = "dotnet test"
}
