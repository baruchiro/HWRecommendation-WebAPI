import pandas as pd


def disable_chained_assignment(func):
    def wrapper(*args, **kwargs):
        save = pd.options.mode.chained_assignment
        pd.options.mode.chained_assignment = None
        result = func(*args, **kwargs)
        pd.options.mode.chained_assignment = save
        return result

    return wrapper
