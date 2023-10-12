class_name GDEnvs
extends Node

enum PoolingStatus { INACTIVE, ACTIVE }

@export var pooling: PoolingStatus = PoolingStatus.INACTIVE;
