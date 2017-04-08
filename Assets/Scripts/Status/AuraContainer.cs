using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for all auras.
public abstract class AuraContainer<T> {

    // Adds aura of that type to this container
    // Returns token, used to identify instance giving this aura
    public abstract int addAura(T value);

    // Removes instance of aura of given token
    public abstract void delAura(int token);

    //Returns value of that aura
    public abstract T getValue();

}
