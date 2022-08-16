using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterNode : MonoBehaviour
{
    
    public TeleporterParent parent;
    public List<GameObject> thingsOnThisNode = new List<GameObject>();


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(ReturnTrueIfObjectShouldBeTeleported(collider.gameObject)){
            parent.TryToTeleportThisPlayer(this, collider.gameObject);
        }
        //collider.gameObject.transform.position = parent.ReturnPairedNode(this.gameObject).transform.position;
        //print(parent.ReturnPairedNode(this.gameObject).name);

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // something just left this collider
        TryToRemoveFromThisNode(collider.gameObject);
    }

    void TryToRemoveFromThisNode(GameObject removeThis){
        try{
            foreach(GameObject m in thingsOnThisNode){
                if(m == removeThis){
                    thingsOnThisNode.Remove(m);
                    return;
                }
            }

        } catch {

        }
    }

    bool ReturnTrueIfObjectShouldBeTeleported(GameObject m){
        // isnt on thingsOnThisNode lise
        foreach(GameObject n in thingsOnThisNode){
            if(n == m){
                return false;
            }

        }
        return true;
    }

    public void ThisPlayerIsArrivingHere(GameObject arrivingPlayer){
        thingsOnThisNode.Add(arrivingPlayer);
    }


}
