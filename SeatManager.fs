module SeatManager

type SeatManager(rows: int, cols: int) =
    // Dictionary to store seat layouts for each showtime
    let mutable seatLayouts = System.Collections.Generic.Dictionary<string, bool[,]>()

    // Initialize seat layout for each showtime
    member this.InitializeShowtime(showtime: string) =
        seatLayouts.[showtime] <- Array2D.create rows cols false

    member this.IsSeatAvailable(showtime: string, row: int, col: int) =
        seatLayouts.[showtime].[row, col] |> not

    member this.ReserveSeat(showtime: string, row: int, col: int) =
        seatLayouts.[showtime].[row, col] <- true

    member this.GetSeatLayout(showtime: string) = seatLayouts.[showtime]