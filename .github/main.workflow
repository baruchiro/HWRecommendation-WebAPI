workflow "Build and Test in PR" {
  resolves = [
    "Run python",
    ".NET Core CLI",
  ]
  on = "pull_request"
}

action ".NET Core CLI" {
  uses = "./.github/net-core/"
  args = "test"
}

action "Run python" {
  uses = "./.github/python/"
  args = "src/test/main.py"
}
