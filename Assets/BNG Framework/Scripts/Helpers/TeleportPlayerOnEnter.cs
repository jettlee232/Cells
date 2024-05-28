<<<<<<< Updated upstream
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class TeleportPlayerOnEnter : MonoBehaviour {

        public Transform TeleportDestination;

        void OnTriggerEnter(Collider other) {
            if(other.GetComponent<CharacterController>()) {
                PlayerTeleport pt = other.transform.GetComponentInParent<PlayerTeleport>();
                if(pt && TeleportDestination) {
                    pt.TeleportPlayerToTransform(TeleportDestination);
                }
            }
        }
    }
}

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class TeleportPlayerOnEnter : MonoBehaviour {

        public Transform TeleportDestination;

        void OnTriggerEnter(Collider other) {
            if(other.GetComponent<CharacterController>()) {
                PlayerTeleport pt = other.transform.GetComponentInParent<PlayerTeleport>();
                if(pt && TeleportDestination) {
                    pt.TeleportPlayerToTransform(TeleportDestination);
                }
            }
        }
    }
}

>>>>>>> Stashed changes
