class_name GDScenes
extends Node

func change_scene(path: String):
    for child in get_children():
        child.queue_free()

    add_child((ResourceLoader.load(path) as PackedScene).instantiate());
