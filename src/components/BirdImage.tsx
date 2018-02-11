import * as React from "react"

interface IBirdProps {
    index,
    onClick,
    photo: Image,
    margin
}

export interface Image {
    id: string,
    src: string,
    original: string,
    width: number,
    height: number,
    caption: string,
    tags: ITag[],
    dateTaken: string,
    locationId: number,
    birdId: number,
    text: string
}

interface ITag {
    value: string,
    title: string
}

export class BirdImage extends React.Component<IBirdProps, {}> {
    constructor(props: IBirdProps) {
        super(props)

        this.state = {

        }
    }
    public render() {
        const cont = {
            backgroundColor: "#eee",
            cursor: "pointer",
            float: "left",
            overflow: "hidden",
            position: "relative",
          } as any

        const imgStyle = {
          display: "block",
          transition: "transform .135s cubic-bezier(0.0,0.0,0.2,1),opacity linear .15s",
        } as any

        return (
            <div style={{ margin: this.props.margin, width: this.props.photo.width, ...cont }}>

              <img style={{ ...imgStyle }} {...this.props.photo}
                   onClick={(e) => this.props.onClick(e, { index: this.props.index, photo: this.props.photo })} />

              <style>
                {`.not-selected:hover{outline:2px solid #06befa}`}
              </style>
            </div>
          )
    }
}
