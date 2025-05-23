/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_ATTACK = 1519321238U;
        static const AkUniqueID PLAY_RUNSTEPS = 1478922094U;
        static const AkUniqueID PLAY_SPECIAL = 2482306727U;
        static const AkUniqueID PLAY_SPIDERCRAWL = 3786703094U;
        static const AkUniqueID PLAY_SPIDERDEATH = 1450764177U;
        static const AkUniqueID PLAY_WIND = 1020223172U;
        static const AkUniqueID STARTLOCATIONSWITCH = 3761343370U;
        static const AkUniqueID STOP_RUNSTEPS = 2362450196U;
        static const AkUniqueID STOP_SPIDERCRAWL = 901179972U;
        static const AkUniqueID STOPALL = 3086540886U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace KEEPCOMBATSTATE
        {
            static const AkUniqueID GROUP = 1017143773U;

            namespace STATE
            {
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID EXPLORE = 579523862U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace KEEPCOMBATSTATE

        namespace KEEPHEALTH
        {
            static const AkUniqueID GROUP = 2937344820U;

            namespace STATE
            {
                static const AkUniqueID KEEPHIGHHEALTH = 2867897876U;
                static const AkUniqueID KEEPLOWHEALTH = 3354185158U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace KEEPHEALTH

        namespace LOCATION
        {
            static const AkUniqueID GROUP = 1176052424U;

            namespace STATE
            {
                static const AkUniqueID KEEP = 2685756106U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID WILDERNESS = 2400105329U;
            } // namespace STATE
        } // namespace LOCATION

        namespace TIMEOFDAY
        {
            static const AkUniqueID GROUP = 3729505769U;

            namespace STATE
            {
                static const AkUniqueID DAY = 311764537U;
                static const AkUniqueID NIGHT = 1011622525U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace TIMEOFDAY

        namespace WILDERNESSCOMBATLEVEL
        {
            static const AkUniqueID GROUP = 3933296273U;

            namespace STATE
            {
                static const AkUniqueID LEVEL0 = 2678230383U;
                static const AkUniqueID LEVEL1 = 2678230382U;
                static const AkUniqueID LEVEL2 = 2678230381U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace WILDERNESSCOMBATLEVEL

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID COMBATSIZE = 2302543530U;
        static const AkUniqueID MX_VOLUME = 1441253587U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINSOUNDBANK = 534561221U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMB = 1117531639U;
        static const AkUniqueID COMBAT = 2764240573U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MX = 1685527054U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID SPIDERCRAWL = 3487890775U;
        static const AkUniqueID STEPS = 1718617278U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
