const std = @import("std");
const Allocator = std.heap.Allocator;
const page_allocator = std.heap.page_allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: []u8) [][]i32 {
    const lines = std.mem.split(u8, input, '\n');
    var first: *i32 = ArrayList(i32).init(alloc);
    var second: *i32 = ArrayList(i32).init(alloc);

    return .{first, second};
}

pub fn main() void {
    const alloc = std.heap.page_allocator;

    const input = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";
    const parsed = parseList(alloc, input);
    std.debug.print("hello\n", .{});
}
