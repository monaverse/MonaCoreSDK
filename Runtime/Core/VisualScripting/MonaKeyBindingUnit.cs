using System.Collections;
using System.Collections.Generic;
using Mona.SDK.Core;
using Mona.SDK.Core.Events;
using Mona.SDK.Core.Utils;
using Unity.VisualScripting;
using UnityEngine;

//Custom node to send the Event
[UnitTitle("MonaKeyBinding")]
[UnitCategory("Events\\Mona\\MonaKeyBinding")]//Setting the path to find the node in the fuzzy finder as Events > My Events.
public class MonaKeyBindingUnit : Unit
{
    [DoNotSerialize]// Mandatory attribute, to make sure we don’t serialize data that should never be serialized.
    [PortLabelHidden]// Hide the port label, as we normally hide the label for default Input and Output triggers.
    public ControlInput inputTrigger { get; private set; }
    [DoNotSerialize]
    public ValueInput enabled;
    [DoNotSerialize]
    [PortLabelHidden]// Hide the port label, as we normally hide the label for default Input and Output triggers.
    public ControlOutput outputTrigger { get; private set; }

    protected override void Definition()
    {
        inputTrigger = ControlInput(nameof(inputTrigger), Trigger);
        enabled = ValueInput<bool>(nameof(enabled), true);
        outputTrigger = ControlOutput(nameof(outputTrigger));
        Succession(inputTrigger, outputTrigger);
    }

    //Send the Event MyCustomEvent with the integer value from the ValueInput port myValueA.
    private ControlOutput Trigger(Flow flow)
    {
        MonaEventBus.Trigger(new EventHook(MonaCoreConstants.KEY_BINDINGS_EVENT), new MonaKeyBindingsEvent() { Enable = flow.GetValue<bool>(enabled) });
        return outputTrigger;
    }
}
