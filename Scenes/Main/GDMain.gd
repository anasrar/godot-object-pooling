extends Node

@export_file("*.tscn") var scene

func _ready():
    (get_node("/root/Scenes") as GDScenes).change_scene(scene);
    queue_free();
