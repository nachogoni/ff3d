using System;
using System.Collections.Generic;

public interface State
{
    bool equals(Object other);
    String toString();
}