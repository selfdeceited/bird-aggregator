import * as React from "react"
import Lightbox from "react-image-lightbox"
import { Link } from "react-router-dom"
import { Image } from "./BirdImage"

interface IBirdLightboxProps {
    photos: Image[],
    index: number,
    onClose: any,
}

interface IBirdLightboxState {
    index: number
}

export class BirdLightbox extends React.Component<IBirdLightboxProps, IBirdLightboxState> {
    private images: string[]

    constructor(props: IBirdLightboxProps) {
        super(props)
        this.state = {
            index: props.index,
        }
        this.images = this.props.photos.map(x => x.original)
    }

    public componentWillReceiveProps(nextProps: IBirdLightboxProps) {
        this.setState({index: nextProps.index})
    }

    public render() {
        function zip<T, U>(list: T[], ...lists: U[][] ): (U|T)[][]  {
            return lists.reduce(
                (previousValue: (U|T)[][], currentValue: (U|T)[]) => 
                      previousValue.map((p, i) => [...p, currentValue[i]])
                , list.map(x=> [x]) as (U|T)[][]
            );
        }

        const photoTitles = zip(
            this.props.photos[this.state.index].caption.split(","), 
            this.props.photos[this.state.index].birdIds)

        const getTitle = () => <div>
            {
                photoTitles.map((arr: (string | number)[], i: number) => <Link
                      className="big-link lightbox-caption"
                      to={`/birds/${arr[1]}`}>
                      {arr[0]}
                      {i === photoTitles.length - 1 ? "" : ","}
                 </Link>,
            )}
        </div>

        const getDescription = () => <div>
            <span>{this.props.photos[this.state.index].text}</span>
            <Link
                key={this.props.photos[this.state.index].id}
                to={"/photos/" + this.props.photos[this.state.index].id}
                role="button"
                className="pt-button pt-minimal pt-icon-arrow-right lightbox-photo-link">
                Visit photo page
            </Link>
        </div>

        const getLightbox = () => (<div>
            <Lightbox
                mainSrc={this.images[this.state.index]}
                nextSrc={this.images[(this.state.index + 1) % this.images.length]}
                prevSrc={this.images[(this.state.index + this.images.length - 1) % this.images.length]}
                onCloseRequest={this.props.onClose}
                onMovePrevRequest={() =>
                    this.setState({
                        index: (this.state.index + this.images.length - 1) % this.images.length,
                    })
                }
                onMoveNextRequest={() =>
                    this.setState({
                        index: (this.state.index + 1) % this.images.length,
                    })
                }
                imageTitle={getTitle()}
                imageCaption={getDescription()}
            />
        </div>)

        return this.state.index === null ? null : getLightbox()
    }
}
