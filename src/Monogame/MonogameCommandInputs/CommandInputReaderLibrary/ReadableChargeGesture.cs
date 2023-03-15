using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public abstract class ReadableChargeGesture : IReadableGesture
    {
        public int Priority { get; set; }

        protected List<Directions.Direction> chargeDirections;
        protected List<Directions.Direction> releaseDirections;

        protected int minChargeTime;
        protected int maxTimeBetweenChargePartitions;
        protected int maxTimeBetweenChargeAndRelease;
        protected int maxTimeAfterRelease;

        public ReadableChargeGesture()
        {
            chargeDirections = new List<Directions.Direction>();
            releaseDirections = new List<Directions.Direction>();
        }
        
        public virtual bool Read(List<ReadablePackage> inputsFacingRight, int currentTime, Directions.FacingDirection facingDirection)
        {
            int currentChargeTimeProgress = 0;

            int releaseTime = currentTime;
            int partitionEndTime = currentTime;
            int lastInputTime = currentTime;

            bool hasReleased = false;
            bool hasStartedCharging = false;
            bool hasStartedPartition = false;
            bool hasEndedPartition = false;


            
            for (int i = inputsFacingRight.Count - 1; i > 0; i--) // start at most recent input and go backwards
            {
                ReadablePackage package = inputsFacingRight[i];
                Directions.Direction packageDirection = package.GetDirectionFacingForward(facingDirection);

                if (!hasReleased)
                {
                    if (currentTime - package.TimeReceived > maxTimeAfterRelease)
                    {
                        // didn't find release in time
                        return false;
                    }

                    // need to look for one of the release directions
                    if (releaseDirections.Any(d => d == packageDirection))
                    {
                        lastInputTime = package.TimeReceived;
                        currentTime = lastInputTime;
                        releaseTime = currentTime;

                        hasReleased = true;
                        continue;
                    }
                }
                else
                {
                    // start counting up charges
                    if (!hasStartedCharging)
                    {
                        bool thisIsTheLastInputWeShouldCheck = (releaseTime - package.TimeReceived > maxTimeBetweenChargeAndRelease);

                        if (chargeDirections.Any(d => d == packageDirection))
                        {
                            hasStartedCharging = true;

                            currentChargeTimeProgress += lastInputTime - package.TimeReceived;

                            lastInputTime = package.TimeReceived;
                            currentTime = lastInputTime;

                            continue;
                        }

                        if (thisIsTheLastInputWeShouldCheck)
                        {
                            // didnt go from charge -> release in the right time period
                            return false;
                        }
                        
                        lastInputTime = package.TimeReceived;
                    }
                    else
                    {
                        //count up further charging, and handle partitioning
                        if (currentChargeTimeProgress >= minChargeTime)
                        {
                            // we did it!!!
                            return true;
                        }


                        if (chargeDirections.Any(d => d == packageDirection))
                        {
                            currentChargeTimeProgress += lastInputTime - package.TimeReceived;

                            currentTime = lastInputTime;

                            if (hasStartedPartition)
                            {
                                hasEndedPartition = true;
                            }
                        }
                        else
                        {
                            // not currently on a direction
                            
                            if (!hasStartedPartition)
                            {
                                hasStartedPartition = true;
                                partitionEndTime = lastInputTime;
                            }


                            if (hasEndedPartition)
                            {
                                //already used up our partition
                                return false;
                            }

                            if (partitionEndTime - package.TimeReceived > maxTimeBetweenChargePartitions)
                            {
                                // partitioned for too long
                                return false;
                            }

                        }

                        lastInputTime = package.TimeReceived;

                    }

                }
            }
            // if we made it through the whole loop without returning, assume we didnt complete the gesture
            return false;
        }
        
    }
}
