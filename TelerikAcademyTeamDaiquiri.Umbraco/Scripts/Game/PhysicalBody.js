 function createPhysicalBody(options) {

     'use strict';

     function move(delta) {

         var self = this,
         	prevX = self.x,
         	prevY = self.y;

         self.x += (self.speed * delta.x);
         self.y += (self.speed * delta.y);
     }

     function colidesWith(otherPhysicalBody) {
         var self = this;
         return (((self.x <= otherPhysicalBody.x && otherPhysicalBody.x <= self.x + self.width) ||
                 (self.x <= otherPhysicalBody.x + otherPhysicalBody.width && otherPhysicalBody.x + otherPhysicalBody.width <= self.x + self.width)) &&
             ((self.y <= otherPhysicalBody.y && otherPhysicalBody.y <= self.y + self.height) ||
                 (self.y <= otherPhysicalBody.y + otherPhysicalBody.height && otherPhysicalBody.y + otherPhysicalBody.height <= self.y + self.height)));

     }

     var physicalBody = {
         x: options.x,
         y: options.y,
         speed: options.speed,
         height: options.height,
         width: options.width,
          move: move,
		colidesWith: colidesWith	
     };

     return physicalBody;

}

