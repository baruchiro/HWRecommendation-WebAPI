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
  args = "src/prepare/main.py data/fake-data-orig.csv data/fake-data-temp.csv"
}
