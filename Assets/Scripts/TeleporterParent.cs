using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleporterParent : MonoBehaviour
{
    //public Dictionary<GameObject, GameObject> nodePairings = new Dictionary<GameObject, GameObject>();
    public float TPCD;
     public GameObject ReturnPairedNode(GameObject why){

        foreach(nodePairings m in nodeLinks){
        
            if( GameObject.ReferenceEquals( m.In , why ) ){
                return m.Out;
            }
        
        }
        return null;

    }

    [Serializable]
    public struct nodePairings {
         public GameObject In;
         public GameObject Out;
         public float timeUsedLast;
    }
    public nodePairings[] nodeLinks;

    //public List<nodePairings> nodeLinks2 = new List<nodePairings>();

    public void TryToTeleportThisPlayer(TeleporterNode InNode , GameObject player){
        // change player location
        // alert the to tile that its about to recieve something
        // to tile then waits until the player leaves to teleport that player again

     
        GameObject arriveNode = ReturnPairedNode(InNode.gameObject);
        arriveNode.GetComponent<TeleporterNode>().ThisPlayerIsArrivingHere(player);
        player.transform.position = arriveNode.transform.position;

       



    }
    


}
