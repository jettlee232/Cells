<<<<<<< Updated upstream
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class AutoGrabGrabbable : GrabbableEvents {
        public override void OnBecomesClosestGrabbable(Grabber touchingGrabber) {
            touchingGrabber.GrabGrabbable(grab);
        }
    }
}

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class AutoGrabGrabbable : GrabbableEvents {
        public override void OnBecomesClosestGrabbable(Grabber touchingGrabber) {
            touchingGrabber.GrabGrabbable(grab);
        }
    }
}

>>>>>>> Stashed changes
