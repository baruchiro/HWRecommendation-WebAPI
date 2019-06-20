workflow "Build and Test in PR" {
  on = "push"
  resolves = [
    "Run python",
    ".NET Core CLI",
  ]
}

action ".NET Core CLI"  {
  uses = "./.github/net-core/"
  args = "test"
}

action "Run python" {
  uses = "./.github/python/"
  args = "src/prepare/main.py data/fake-data-orig.csv data/fake-data-temp.csv"
}
