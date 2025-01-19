const std = @import("std");
const dbgPrt = std.debug.print;

pub fn main() !void  {
    const arena = std.heap.ArenaAllocator.init(std.heap.page_allocator);
    defer arena.deinit();
    //const alloc = arena.allocator();

    var table: [4096:0]u8 = undefined;
    const reader = std.io.getStdIn().reader();

    while (try reader.read(&table)) |len| {
        const slice = table[0..len-1];
        dbgPrt("{s}\n", .{ slice, });
    } else |err| {
        switch (err) {
            ReadError.EndOfStream => break
        }
    }



}
