workflow "Build and Test in PR" {
  on = "push"
  resolves = ["docker://python"]
}

action ".NET Core CLI" {
  uses = "./"
  args = "test"
}

action "docker://python" {
  uses = "docker://python"
  needs = [".NET Core CLI"]
  runs = "python"
  args = "DataPreparing/src/prepare/main.py"
}
