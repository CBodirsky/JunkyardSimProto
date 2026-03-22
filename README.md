# JunkyardSimProto
A Unity prototype exploring physics‑driven item interaction, container logic, and dynamic object processing inspired by games like Cash Cleaner Simulator and Miner Mogule.

Project Overview

This prototype demonstrates:

    A player interaction system (picking up, dropping, mining, and processing items)

    A physics‑safe item pipeline (Free → Held → Stored → Processed)

    A container system that slots items visually without physics overlap

    A shredder system that converts items into material bits

    A stable physics architecture that avoids common Unity pitfalls
    (kinematic toggling, collider desync, rigidbody wake‑up issues)

The goal is to show real gameplay engineering, not a finished game.
Core Features
1. Player Interaction System

    Raycast‑based mining and item pickup

    Held items have physics disabled safely

    Dropped items re‑enter physics cleanly

    Prevents jitter, ghost rigidbodies, and collider desync

2. Cash Cleaner–Style Container System

    Items snap into predefined slot paths inside crates

    No physics while stored

    Overflow items fall out naturally

    Supports messy, organic layouts with slight randomization

    Crates remain stable when picked up or dropped

3. Shredder / Processing System

    Items entering the shredder trigger a processing event

    Items are converted into material bits

    Fully physics‑safe: no double‑processing, no ghost items

    Works with held, dropped, or thrown items

4. Physics Stability Architecture

This project solves several tricky Unity physics issues:

    Rigidbody wake‑up after kinematic toggle

    Collider re‑enable ordering

    Parenting under rigidbodies

    Trigger detection failures

    Items falling through the floor

    Stored items reactivating physics unexpectedly

The code demonstrates robust handling of Unity’s physics lifecycle.
Technical Highlights
Item State Machine

Items move through three controlled states:
Code

Free → Held → Stored → Processed

Each state has:

    collider rules

    rigidbody rules

    parenting rules

    trigger behavior

This prevents physics conflicts and keeps the system predictable.
Slot Path System

Instead of a grid, crates use a list of transforms that define where items appear.
This allows:

    handcrafted or procedural layouts

    messy but readable visuals

    easy tuning per crate type

    zero physics overlap

Clean, Modular Code

    Each system is isolated and testable

    No monolithic “god scripts”

    Prefab‑driven architecture

    Clear separation of visuals vs physics vs logic

Project Structure
Code

/Scripts
    /Interaction
    /Items
    /Containers
    /Processing
/Prefabs
/Scenes
/Art
/Materials

Current Status

This is an in‑progress prototype, not a full game.
The core systems are functional, but several areas are intentionally unfinished:

    No UI or progression systems

    No save/load

    Limited item variety

    No final art or sound

    No optimization pass

The purpose of this upload is to demonstrate gameplay engineering, not content completeness.
Future Improvements (Planned / Possible)

    Expand slot path generator (spiral, layered, randomized)

    Add more item types and processing recipes

    Add conveyor belts and automated sorting

    Add crate stacking and storage slotting

    Add visual polish (particles, sounds, animations)

    Convert systems into reusable Unity packages

Why This Project Exists

This prototype was built to explore:

    Unity physics edge cases

    Interaction design

    Container systems

    State machines

    Clean gameplay architecture

    Debugging complex object lifecycles

It serves as a portfolio piece demonstrating systems thinking, problem‑solving, and real gameplay engineering beyond simple tutorials.
License

MIT License
